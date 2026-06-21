<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Licencas.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.Licencas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">
        function OnPossuiDtFimChanged(ckPossuiDtFim) {
            if (typeof ckPossuiDtFim != 'undefined') {
                if (typeof ckPossuiDtFim.GetValue() != 'undefined' && ckPossuiDtFim.GetValue() != null) {

                    if (ckPossuiDtFim.GetValue().toString() == "S") {
                        var editorPeriodoLimite = grdLicencas.GetEditor("periodo_limite");
                        editorPeriodoLimite.inputElement.readOnly = false;
                        participacontratotemporario.SetChecked(false);
                        participacontratotemporario.SetEnabled(false);
                    } else if (ckPossuiDtFim.GetValue().toString() != "S") {
                        var editorPeriodoLimite = grdLicencas.GetEditor("periodo_limite");

                        editorPeriodoLimite.inputElement.readOnly = true;
                        editorPeriodoLimite.inputElement.value = "";
                        participacontratotemporario.SetEnabled(true);
                    }
                }
            }
        }      

    </script>

    <asp:ObjectDataSource ID="odsLicencas" runat="server" TypeName="Techne.Lyceum.RN.Licencas"
        SelectMethod="ConsultarLicencas" InsertMethod="InsertMethod" UpdateMethod="UpdateMethod"
        DeleteMethod="DeleteMethod" />
    <dxwgv:ASPxGridView ID="grdLicencas" runat="server" ClientInstanceName="grdLicencas"
        AutoGenerateColumns="False" DataSourceID="odsLicencas" KeyFieldName="motivo"
        OnRowInserting="grdLicencas_RowInserting" OnRowUpdating="grdLicencas_RowUpdating"
        OnRowDeleting="grdLicencas_RowDeleting" OnCellEditorInitialize="grdLicencas_CellEditorInitialize"
        OnInitNewRow="grdLicencas_InitNewRow" OnRowValidating="grdLicencas_RowValidating"
        OnStartRowEditing="grdLicencas_StartRowEditing">
        <SettingsBehavior ConfirmDelete="True" />
        <SettingsEditing Mode="Inline" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <Columns>
            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
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
            <dxwgv:GridViewDataTextColumn Caption="Licença*" HeaderStyle-Font-Bold="true" FieldName="motivo"
                VisibleIndex="1" Width="150px">
                <PropertiesTextEdit Width="150px" MaxLength="20">
                    <ValidationSettings CausesValidation="True">
                        <RequiredField ErrorText="Favor informar a licença." IsRequired="True" />
                        <RegularExpression ErrorText="Licença deve conter apenas letras e números." ValidationExpression="^[+]?[\w]*$" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Descrição*" HeaderStyle-Font-Bold="true" FieldName="descricao"
                VisibleIndex="2" Width="300px">
                <PropertiesTextEdit Width="300px" MaxLength="100">
                    <ValidationSettings CausesValidation="True">
                        <RequiredField ErrorText="Favor informar a descrição." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataCheckColumn Caption="Possui Data Final?" FieldName="possui_dtfim"
                VisibleIndex="3">
                <PropertiesCheckEdit ValueChecked="S" ValueType="System.String" ValueUnchecked="N"
                    DisplayTextChecked="Sim" DisplayTextUnchecked="Não">
                    <ClientSideEvents CheckedChanged="function(s, e) { OnPossuiDtFimChanged(s);}" Init="function(s, e) { OnPossuiDtFimChanged(s);}" />
                </PropertiesCheckEdit>
            </dxwgv:GridViewDataCheckColumn>
            <dxwgv:GridViewDataTextColumn Caption="Período Limite (em dias)" FieldName="periodo_limite"
                VisibleIndex="4" UnboundType="Decimal">
                <PropertiesTextEdit MaxLength="5">
                  <ClientSideEvents KeyPress="function (s, e){ SomentePermitirNumeros(s, e.htmlEvent); }" />
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                      <%--  <RequiredField ErrorText="Favor informar o código." IsRequired="True" />--%>
                        <RegularExpression ErrorText="O campo Código só aceita números inteiros e positivos."
                            ValidationExpression="^[+]?\d*$" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataCheckColumn Caption="Bloqueia GLP?" FieldName="bloqueia_glp" VisibleIndex="5">
                <PropertiesCheckEdit ValueChecked="S" ValueType="System.String" ValueUnchecked="N"
                    DisplayTextChecked="Sim" DisplayTextUnchecked="Não" />
            </dxwgv:GridViewDataCheckColumn>
            <dxwgv:GridViewDataCheckColumn Caption="Participa Contrato Temporário?" FieldName="participacontratotemporario"
                VisibleIndex="5">
                <PropertiesCheckEdit ValueChecked="S" ValueType="System.String" ValueUnchecked="N"
                    DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ClientInstanceName="participacontratotemporario" />
            </dxwgv:GridViewDataCheckColumn>
             <dxwgv:GridViewDataCheckColumn Caption="Valida Alocação?" FieldName="validaalocacao"
                VisibleIndex="6">
                <PropertiesCheckEdit ValueChecked="S" ValueType="System.String" ValueUnchecked="N"
                    DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ClientInstanceName="validaalocacao" />
            </dxwgv:GridViewDataCheckColumn>
        </Columns>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
    <asp:Button ID="Button1" runat="server" Text="Exportar" OnClick="Button1_Click_ExportarButton1_Click" />
</asp:Content>
