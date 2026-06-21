<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Turno.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.Turno"
    MasterPageFile="~/Modulos/LyceumMaster.Master" %>

<%@ Register assembly="DevExpress.Web.ASPxEditors.v9.2" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>

<asp:Content ID="conFormularioHorarioOperacional" ContentPlaceHolderID="cphFormulario"
    runat="server">
        <script>
        function ValidaHora(txtHora) {
            if (txtHora.GetValue() == null) return false;
            {
                var horaIni = txtHora.GetValue();
                var tO = [];
                var intminute;
                var hora;
                var minute;

                tO = horaIni.split(':');
                if ((tO[0] == "") || (tO[0] == " ") || (tO[0] == "  ")) return false;
                if ((tO[1] == "") || (tO[1] == " ") || (tO[1] == "  ")) return false;
                hora = parseInt(tO[0]);
                minute = parseInt(tO[1]);

                if (hora == NaN) return false;
                if ((hora < 0) || (hora > 23)) return false;
                if (minute == NaN) return false;
                if ((minute < 0) || (minute > 59)) return false;
                return true;
            }
        }

    </script>
    
    <techne:TTableDataSource ID="tdsTurno" runat="server" DataTableClassName="Techne.Lyceum.CR.Ly_turno"
        SqlOrder="descricao">
    </techne:TTableDataSource>

    <dxwgv:ASPxGridView ID="grdTurno" runat="server" AutoGenerateColumns="False" DataSourceID="tdsTurno"
        ClientInstanceName="grdTurno" KeyFieldName="turno" Width="1020px" OnCellEditorInitialize="grdTurno_CellEditorInitialize"
        Font-Names="Verdana" Font-Size="Small" OnInitNewRow="grdTurno_InitNewRow" OnStartRowEditing="grdTurno_StartRowEditing"
        OnAfterPerformCallback="grdTurno_AfterPerformCallback" 
        onrowvalidating="grdTurno_RowValidating" 
        onrowinserted="grdTurno_RowInserted">
        <SettingsBehavior ConfirmDelete="True" />
        <SettingsEditing Mode="Inline" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <Columns>
            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer" onclick="grdTurno.AddNewRow();"
                            alt="Novo" />
                    </div>
                </HeaderCaptionTemplate>
               <%--  <EditButton Text="Editar" Visible="True">
                    <Image Url="~/img/bt_editar.png" />
                </EditButton>--%>
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
            <dxwgv:GridViewDataTextColumn Caption="Turno*" FieldName="turno" HeaderStyle-Font-Bold="true"
                VisibleIndex="1" Width="200px">
                <PropertiesTextEdit MaxLength="20" Width="200px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar o Turno." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Abreviatura*" HeaderStyle-Font-Bold="true"
                FieldName="mnemonico" VisibleIndex="2">
                <PropertiesTextEdit MaxLength="2" Width="100px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar a Abreviatura." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            
            <dxwgv:GridViewDataTextColumn Caption="Hora Início*" FieldName="horainicio" Name="horainicio"
                VisibleIndex="3" Width="70px">
                <PropertiesTextEdit ValidationSettings-Display="Dynamic">
                    <ValidationSettings Display="Dynamic">
                    </ValidationSettings>
                </PropertiesTextEdit>
                <DataItemTemplate>
                    <dxe:ASPxTextBox ID="txtBoxIni" runat="server" NullText='<%# Container.VisibleIndex.ToString() %>'
                        ClientInstanceName='<%# "txtBoxIni"+Container.VisibleIndex.ToString() %>' Value='<%# Eval("horainicio","{0:HH:mm}") %>'
                        Width="60px" MaxLength="5" DisplayFormatString="HH:mm">
                        <MaskSettings Mask="##:##" />
                        <ClientSideEvents LostFocus="function(s, e) { ChangeHoraFim(s,e); }" />
                    </dxe:ASPxTextBox>
                </DataItemTemplate>
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dxwgv:GridViewDataTextColumn>
            
            <dxwgv:GridViewDataTextColumn Caption="Hora Fim*" FieldName="horafim" Name="horafim"
                VisibleIndex="4" Width="70px">
                <PropertiesTextEdit ValidationSettings-Display="Dynamic">
                    <ValidationSettings Display="Dynamic">
                    </ValidationSettings>
                </PropertiesTextEdit>
                <DataItemTemplate>
                    <dxe:ASPxTextBox ID="txtBoxFim" runat="server" NullText='<%# Container.VisibleIndex.ToString() %>'
                        ClientInstanceName='<%# "txtBoxFim"+Container.VisibleIndex.ToString() %>' Value='<%# Eval("horafim","{0:HH:mm}")%>'
                        Width="60px" MaxLength="5" DisplayFormatString="HH:mm">
                        <MaskSettings Mask="##:##" />
                        <ClientSideEvents LostFocus="function(s, e) { ChangeHoraFim(s,e); }" />
                    </dxe:ASPxTextBox>
                </DataItemTemplate>
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dxwgv:GridViewDataTextColumn>

            
            
            


            <dxwgv:GridViewDataTextColumn Caption="Descrição*" HeaderStyle-Font-Bold="true" FieldName="descricao"
                Width="720px" VisibleIndex="5">
                <PropertiesTextEdit MaxLength="100" Width="720px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar a Descrição." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
        </Columns>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
    <p align="center">
        <dxe:ASPxButton ID="btnSalvar" runat="server" Text="Salvar" Visible="true" AutoPostBack="true"
            OnClick="btnSalvarGrade_Click">
        </dxe:ASPxButton>
    </p>      
</asp:Content>
